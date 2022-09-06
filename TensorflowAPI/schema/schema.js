const graphql = require('graphql');
const _ = require('lodash')
const jobstatus = require('../models/jobstatus');
var MongoClient = require('mongodb').MongoClient;
require('dotenv').config() ;



const { GraphQLObjectType, GraphQLString, GraphQLID, GraphQLList, GraphQLSchema, GraphQLNonNull } = graphql;

var listofjobsdriverhascompleted ;
var listofJobsDriverhastodo ;


function joblist(driverid){
    console.log(driverid);
    MongoClient.connect(process.env.MONGODB_URL, function (err, db) {
        if(err) throw err;
        var dbo = db.db("barride");
        var query = { driverid: driverid };  
        dbo.collection("jobstatus").find(query).toArray(function(err, result) {
           if (err) throw err;
           listofjobsdriverhascompleted =  result ;
               // methana adaal afiltering process eka karanna. 
            
               
         });   
         
         // methanin api toragannwa driver kenek asign wecchi nathi list eka 
         var query = { driverid: null };  
        dbo.collection("jobstatus").find(query).toArray(function(err, result) {
           if (err) throw err;
           listofJobsDriverhastodo =  result ;
               
         });   
         
         
   });
   return listofJobsDriverhastodo ;
}





const jobstatusType = new GraphQLObjectType({
    name: 'jobstatus',
    fields: ( ) => ({
        _id: { type: GraphQLID },
        customerid: { type: GraphQLString },
        driverid: { type: GraphQLString },
        customerFrom: { type: GraphQLString },
        customerTo: { type: GraphQLString },
        isJobdone: { type: GraphQLString },
        amountPayable: { type: GraphQLString },
        vehicleType: { type: GraphQLString },
        feedbackRatefordriver: { type: GraphQLString },
        feedbackrateforcustomer: { type: GraphQLString },
        feedbackforcustomer: { type: GraphQLString },
        feedbackfordriver: { type: GraphQLString },
        dateTime: { type: GraphQLString },
        paywith: { type: GraphQLString },
        Nonce :{ type: GraphQLString},
        
    })
});




const RootQuery = new GraphQLObjectType({
    name: 'RootQueryType',
    fields: {
        status: {
            type: GraphQLString,
            resolve(parent, args){
                return "Welcome to GraphQL"
            }
        },
        allJobs: {
            type: new GraphQLList(jobstatusType), 
            args: {
                driverid:{type: new GraphQLNonNull(GraphQLString)}
            },          
            resolve(parent,args){
                return joblist( args.driverid )
            }
        },
        
    }
});



module.exports = new GraphQLSchema({
    query: RootQuery,
    
});