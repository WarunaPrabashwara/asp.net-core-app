const express = require('express');
const {graphqlHTTP} = require('express-graphql');
const schema = require('./schema/schema');

const mongoose = require('mongoose');
require('dotenv').config()
const https = require('https');
const fs = require('fs');
const cors = require('cors');

const app = express();



app.use(cors( {
  origin: 'https://localhost:44384'    
}));

mongoose.connect(process.env.MONGODB_URL, { useNewUrlParser: true, useUnifiedTopology: true });
mongoose.connection.once('open', ()=>{
    console.log("Connected to MongoDB")
})

app.use(  "/graphql",  graphqlHTTP({ schema: schema, graphiql: true, }));  

app. use('/',(req,res) => {
    res.send("Welcome to GraphQL server. Use GraphQL endpoint at /graphql")
})

const options = {
    key: fs.readFileSync('sslcertificates/key.pem'),
    cert: fs.readFileSync('sslcertificates/cert.pem')
  };


//var httpsServer = https.createServer(options, app);

// httpsServer.listen(5000, () => {    console.log("server starting on port : 5000")  });

app.listen(5000, () => {
    console.log("server starting on port : 5000")
    
  });
