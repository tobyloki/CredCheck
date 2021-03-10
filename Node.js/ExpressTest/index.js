require('dotenv').config();

const bodyParser = require('body-parser');
const express = require('express');
const jwt = require('jsonwebtoken');
const crypto = require('crypto');
const app = express();
const rds = require('./rds.js');

const refreshToken = process.env.REFRESH_TOKEN;
const issuer = 'CredCheck';

const port = process.env.PORT || 3000;

rds.init();

app.use(bodyParser.json());
// app.use((req, res, next) => {
//     res.header('Content-Type', 'application/json');
//     res.header("Access-Control-Allow-Origin", "*");
//     res.header("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT");
//     res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
//     next();
// });
app.listen(port, () => console.log(`App listening on http://localhost:${port}`));

app.get('/', (req, res) => {
    try {
        const token = jwt.sign(
            {
                email: req.query.email
            },
            refreshToken,
            {
                expiresIn: '1h',
                issuer
            }
        );
        console.log(`[${req.email}] - Created token: ${token}`);

        // res.send(req.query.email);

        res.send({
            email: req.query.email,
            token
        });
    } catch(e) {
        res.send(e);
    }
});

app.get('/email', async (req, res) => {
    try {
        const ret = {};
        const token = await validToken(req.headers.authorization);
        
        if(token.valid) {
            ret.email = token.content.email;
        }

        res.send(ret);
    } catch(e) {
        res.send(e);
    }
});

app.get('/test', async (req, res) => {
    try {
        const ret = {};

        ret.res = await rds.query('SELECT * FROM users;');

        res.send(ret);
    } catch(e) {
        res.send(e);
    }
});

async function validToken(token) {
    return new Promise((resolve, reject) => {
        jwt.verify(token, refreshToken,
            {
                issuer
            },
            (err, res) => {
                if(err) {   // Tested: succesfully throws jwt expired & invalid key
                    console.log('Err: ' + err);
                    reject(err);
                } else {
                    console.log('Res: ' + JSON.stringify(res, null, 2));
                    resolve({
                        valid: true,
                        content: res
                    });
                }
            }
        );
    });
}