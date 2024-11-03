# SystemPrototype
We work with a lot of cloud components and variety of protocols on a daily basis. But do we really understand what it takes to implement these? This repo is an attempt to demystify large concepts into their core via an example-driven approach.

## QueryDBShard
The `QueryDBShard` program demonstrates how to manage database connections across multiple shards. It uses a static mapping of user IDs to shard connection strings to prototype how to query data sharded across different DBs. 

### How to Populate the Database
Before running the `QueryDBShard` program, you need to populate the `posts` table in the `baishali_test` database. You can do this by executing the `populate_posts.sh` script provided in the `preReq` directory. 

To populate the database, follow these steps:
1. Ensure you have MySQL installed and running.
2. Update the `DB_PASS` variable in the `populate_posts.sh` script with your MySQL root password if necessary.
3. Run the script using the following command:
   ```bash
   bash preReq/populate_posts.sh
   ```

This will create the `posts` table and insert sample data for 10 users, allowing you to test the `QueryDBShard` functionality.
