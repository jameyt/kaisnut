// var express = require('express');
// var app = express();
// var cool = require('cool-ascii-faces');

// app.set('port', (process.env.PORT || 5000));
// app.use(express.static(__dirname + '/public'));

// app.get('/', function(request, response) {
//   response.send('Hello World!');
// });

// app.listen(app.get('port'), function() {
//   console.log("Node app is running at localhost:" + app.get('port'));
// });

//start here

// var express = require('express')
// var app = express();
// var cool = require('cool-ascii-faces');

// app.set('port', (process.env.PORT || 5000))

//end here

// app.get('/', function(request, response) {
//   response.send(cool());
// });

// app.get('/', function(request, response) {
//   var result = ''
//   var times = process.env.TIMES || 5
//   for (i=0; i < times; i++)
//     result += cool();
//   response.send(result);
// });

//start here

// var pg = require('pg');

// app.get('/db', function (request, response) {
//   pg.connect(process.env.DATABASE_URL, function(err, client, done) {
//     client.query('SELECT * FROM test_table', function(err, result) {
//       done();
//       if (err)
//        { console.error(err); response.send("Error " + err); }
//       else
//        { response.send(result.rows); }
//     });
//   });
// })

// app.listen(app.get('port'), function() {
//   console.log("Node app is running at localhost:" + app.get('port'))
// })

//end here

var express = require('express');
var app = express();

app.use(express.static(__dirname + '/public'));

app.listen(process.env.PORT || 3000);