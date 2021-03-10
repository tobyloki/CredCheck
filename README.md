# CredCheck

### Security
- contains documents about our cybersecurity research
- Burp Suite brute force attack
	- Cluster Bomb: for 2+ params (payloads)
	- Sniper: for 1 param (payload)
- Gobuster Commands
```cmd
gobuster dir -u http://localhost:3000/ -w words.txt -x php,txt,html
gobuster dir -u http://localhost:3000/index -w words.txt -x php,txt,html
gobuster dir -u http://localhost:3000/error -w words.txt -x php,txt,html
```
- Juice Shop SQL injection tests
	- The website's login functionality does not parameterize the SQL query, so the email and password are directly inserted into the query. As a result, I am able to manipulate the query to login as Jim by knowing his email but not his password. I do this by inputting his email, followed by a ' to close the query and -- to comment out the rest of the query, including the part that checks if the password is correct. By typing anything into the password field (since it also checks if the field is empty), I can log in and access Jim's account.
- Example XSS on Juice Shop
	- query param `q` contains XSS
	- plays song when pressing play
	- https://demo.owasp-juice.shop/#/search?q=%3Ciframe%20width%3D%22100%25%22%20height%3D%22166%22%20scrolling%3D%22no%22%20frameborder%3D%22no%22%20allow%3D%22autoplay%22%20src%3D%22https:%2F%2Fw.soundcloud.com%2Fplayer%2F%3Furl%3Dhttps%253A%2F%2Fapi.soundcloud.com%2Ftracks%2F771984076%26color%3D%2523ff5500%26auto_play%3Dtrue%26hide_related%3Dfalse%26show_comments%3Dtrue%26show_user%3Dtrue%26show_reposts%3Dfalse%26show_teaser%3Dtrue%22%3E%3C%2Fiframe%3E

### ASP.NET
- Testing
	- CrudSite
		- create, read, update, delete pages on SQLServer
	- Sessions
		- uses sessions to manage current user state and authorization
	- TransactionApp
		- test site that interacts with BitPay payment gateway
- API
	- made w/VStudio
	- The Api works as the middleman , for the users to access the database through http requests that are called using the web application. 
	- The Api listens for the requests are provides a services response object, to indicate sucess/failure, and to return any requested or necessary  data
	- Api is provided parameteres to the SQL commands, so as to avoid SQL injections
	- Additionally, the web application checks the formatting of the values that will be going into the sql command.
	- HTTP requests that require user input are provided as a json body, and ids of the card are provided in the url.
	- When a card is initially entered into the database, sha256 is used to create a unique string identifier for the card.
	- The identifiers is made up of the credentials of the card.
	- The database is using MYSQL and amazon's RDS feature to access it over the web.
- webApp
	- made w/VSCode
	- Over the course of this term I learned to use ASP.NET and Docker. I created an ASP.NET website that accepts form input for a user’s credit card information. This information is then validated to ensure that all fields contain valid information. For the card number specifically, I used the Luhn checksum algorithm to verify that the card number is valid. If the data is valid, then it is then sent to Jonathan’s API via a post request where it is stored in the database.
	- I also utilized Docker to run my ASP.NET website inside a docker container. By creating an image of this container, my website can then be run. Doing this had the benefit of increasing the performance and portability of my website, as well as isolating it from other applications. Finally, I also researched a handful of cybersecurity aspects.I learned about the prevention of SQL injections and Cross Site Scripting (XSS). I was able to learn about these flaws in a more hands on way by using the OWASP Juice Shop website. In addition, I also learned about these by participating in a few tasks for the Advent of Cyber challenge on the website TryHackMe. Through this website, I also learned about the TCP/IP's three-way-handshake and how the Nmap tool can be used for penetration testing against a server to scan for active IPs and ports on a network and then analyzing IP packets for information about them.

### TryHackMe Notes
- SQL Injections
	- I learned about SQL injections through tasks on the TryHackMe website and then tested it on the OWASP Juice Shop website to log in to another person’s account on the website, by commenting out the part of the query that verifies the password.
- Cross Site Scripting (XSS)
	- Like with SQL injections, I learned about Cross Site Scripting through tasks on the TryHackMe website and then tested it on the OWASP Juice Shop website to embed my own scripts into the website, including an iframe that played a song.
- Nmap:
	- I also learned about the TCP/IP's three-way-handshake and how the Nmap tool can be used for penetration testing against a server to scan for active IPs and ports on a network and then analyzing IP packets for information about them.
- Go Buster:
   - Gobuster was used to attempt to access pages in our website that were lazily kept as accessible to users, including possibly valuable information that users should not be able to see.
   - Running gobuster required a large txt file for common pagenames that are often used to store valuable information during development
   - Running the gobuster provided some files, but we made an effor to limit the amount of accessible pages to users.
- WFuzz:
   - WFuzz is similart to go-buster, in that the hacker provides a text file, but it also allows hackers to attept various credential combinations for loging in as another user or admin.
   - We did not test this functionality with our web application, since we did not have a login feature in our application
- OWasp Zap:
   - This program provides a thoruogh trial on the webpage that is being attacked, but it is used more often by the developers as a way to get warning signs of the vulnerabilities of their page.
   - The scan using Owasp showed various alerts that we were not aware of, and further learning could be done.
- Priviledge escalation:
   - This test is used to check if the priviledges on a user can be exploited to gain additional priviledges, by traversing the priviledges of users in the same level as you, or as moving up the rung of privlidges by getting admin privilidges,
   - We were able to use priviledge escelation on a challenge found online, but there was no concrete method to use this in our system.

### Node.js
- PaymentGateway
	- simple test of jwt token and nonce for mimicking Braintree payment gateway
- ExpressTest
	- simple express app to get and authenticate jwt token

### Kubernetes
- documentation for setting up Kubernetes on AWS EKS

### Design
- Bootstrap Studio and Adobe XD design of add card page

### Wireshark
- saved captures of MySQL traffic

# Extra Stuff
### Braintree
- https://developers.braintreepayments.com/start/overview
- Nonce
	- https://developers.braintreepayments.com/guides/payment-method-nonces
	- A payment method nonce may only be used once. If it is not used, it expires 3 hours after being created.
	- allows your server to communicate sensitive payment information to Braintree without ever touching the raw data.

### Setting up wafw00f
- https://github.com/EnableSecurity/wafw00f
1. git clone https://github.com/EnableSecurity/wafw00f.git
2. cd wafw00f
3. python setup.py install
4. cd wafw00f/bin
5. python wafw00f example.com

### Links
- https://www.youtube.com/watch?v=wIRVn2dZkaA
- https://stackoverflow.com/questions/7073484/how-do-css-triangles-work
- https://www.youtube.com/watch?v=10SwsoYNkVc&t=28s
- https://www.npmjs.com/package/jsonwebtoken
- https://developers.braintreepayments.com/start/overview
- https://jwt.io/
- https://www.youtube.com/watch?v=mbsmsi7l3r4
- https://developers.braintreepayments.com/reference/general/testing/node
- https://medium.com/@cengizyilmaz/nonce-solution-for-jwt-f23ef0cb494#:~:text=When%20the%20request%20iss%20sending,and%20passed%20through%20same%20way.
- https://www.npmjs.com/package/redis
- https://docs.google.com/document/d/1pafmeHDOKkVNVYmW93X2RWCESkVF142q-tkHv0qq4ag/edit?ts=5ffde475
- https://www.youtube.com/watch?v=V0_RPT6HsE4
- https://osqa-ask.wireshark.org/questions/27515/how-do-i-view-a-raw-http-requestresponse
- https://codeforgeek.com/encrypt-and-decrypt-data-in-node-js/
- https://www.youtube.com/watch?v=2PPSXonhIck
- https://stackoverflow.com/questions/14168703/crypto-algorithm-list
- https://docs.google.com/document/d/1rY-XN-GDorIOF61kzWXbArD_ypwfdvHA009TFNJptco/edit?ts=6008731b
- https://www.youtube.com/watch?v=OH6Z0dJ_Huk
- https://www.youtube.com/watch?v=YCCrVtvAu2I&list=PLBf0hzazHTGO3EpGAs718LvLsiMIv9dSC
- https://juice-shop.herokuapp.com/#/
- https://nordicapis.com/5-ways-to-hack-an-api-and-how-to-defend/
- https://portswigger.net/web-security/request-smuggling/exploiting
- https://www.valencynetworks.com/blogs/cyber-attacks-explained-database-hacking/
- https://www.anitian.com/hacking-microsoft-sql-server-without-a-password/
- Eks
	- https://www.youtube.com/watch?v=c4WcYjama6U
	- https://aws.amazon.com/premiumsupport/knowledge-center/eks-kubernetes-services-cluster/
	- https://docs.aws.amazon.com/eks/latest/userguide/getting-started-eksctl.html
	- https://stackoverflow.com/questions/60013670/eks-create-cluster-command-fails-with-error-computing-fingerprint-illegal-base6
- https://tryhackme.com/room/learncyberin25days
- https://tryhackme.com/room/openvpn
- https://gchq.github.io/CyberChef/#recipe=To_Hex('None',0)&input=eyJjb21wYW55IjoiVGhlIEJlc3QgRmVzdGl2YWwgQ29tcGFueSIsICJ1c2VybmFtZSI6InNhbnRhIn0
- https://security.stackexchange.com/questions/145392/how-can-you-steal-cookies-from-chrome/182533
- https://www.youtube.com/watch?v=wQSuZFd01tY&t=1836s