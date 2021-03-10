require('dotenv').config({path: '../.env'});

const jwt = require('jsonwebtoken');
const crypto = require('crypto');
// const NodeCache = require( "node-cache" );
// const cache = new NodeCache();
const redis = require("redis");

const client = redis.createClient({
    port: 6379,
    host: '127.0.0.1'
});
client.on("error", (error) => {
  console.error(error);
});

const refreshToken = process.env.REFRESH_TOKEN;
const nonceKey = process.env.NONCE_KEY;

// generateCredentials();
main();

async function generateCredentials() {
    const refreshToken = crypto.randomBytes(32).toString('base64');
    const nonceKey = crypto.randomBytes(32).toString('base64');

    console.log('refreshToken: ' + refreshToken);
    console.log('nonceKey: ' + nonceKey);
}

async function main() {

    const issuer = 'CredCheck';
    const userId = 'myuserid';
    const cardId = 'mycardid';
    const cardNonce = cardId + '-nonce';

    // create token
    const token = jwt.sign(
        {
            id: userId,
            name: 'Jacob'
        },
        refreshToken,
        {
            expiresIn: 10,  // expires in 10s,
            issuer,
            header: {
                version: 1
            }
        }
    );
    console.log('token: ' + token);

    // decode token
    const decodedToken = jwt.decode(token, {complete: true});
    console.log('decodedToken: ' + JSON.stringify(decodedToken, null, 2));

    // verify token against key
    jwt.verify(token, refreshToken,
        {
            issuer
        },
        (err, res) => {
            if(err) {   // Tested: succesfully throws jwt expired & invalid key
                console.log('Err: ' + err);
            } else {
                console.log('Res: ' + JSON.stringify(res, null, 2));
            }
        }
    );

    // create a nonce - only used once, expires after 3 hrs
    // it basically looks like a jwt token, but w/out payload
    const nonce = encrypt(JSON.stringify({
        cardId,
        date: new Date()
    }));
    console.log('nonce: ' + nonce);

    // set nonce in cache
    // const success = cache.set(cardNonce, nonce, 10 );
    client.set(cardNonce, nonce, redis.print);
    client.expire(cardNonce, 5);    // ttl is in seconds

    // await delay(6000);  // tested: redis cache ttl works

    // get nonce from cache
    // const cachedNonce = cache.get(cardNonce);
    const cachedNonce = await getValue(cardNonce);
    console.log('cachedNonce: ' + cachedNonce);

    // decode nonce
    const decodedNonce = JSON.parse(decrypt(cachedNonce));
    console.log('decodedNonce: ' + JSON.stringify(decodedNonce, null, 2));

    // delete nonce in cache
    client.del(cardNonce);

    // const hash = encrypt({asdf:2});
    // console.log(hash);

    // const res = decrypt(hash);
    // console.log(res);
    
    client.quit();
}

function encrypt(text) {
    const iv = crypto.randomBytes(16);
    const cipher = crypto.createCipheriv('aes-256-cbc', Buffer.from(nonceKey, 'base64'), iv);
    const data = Buffer.concat([cipher.update(text), cipher.final()]);
    const res = Buffer.from(JSON.stringify({
        iv: iv.toString('base64'),
        data: data.toString('base64')
    })).toString('base64');
    return res;
}

function decrypt(text) {
    const data = JSON.parse(Buffer.from(text, 'base64').toString('ascii'));
    const iv = Buffer.from(data.iv, 'base64');
    const encryptedText = Buffer.from(data.data, 'base64');
    const decipher = crypto.createDecipheriv('aes-256-cbc', Buffer.from(nonceKey, 'base64'), iv);
    let decrypted = decipher.update(encryptedText);
    decrypted = Buffer.concat([decrypted, decipher.final()]);
    return decrypted.toString();
}

function getValue(key) {
    return new Promise((resolve, reject) => {
        client.get(key, (err, res) => {
            if(err){
                console.log('Err: ' + err);
                reject(err);
            } else {
                resolve(res);
            }
        });
    });
}

function delay(timeout) {
    return new Promise(resolve => setTimeout(resolve, timeout));
}