#!/bin/bash

# Database credentials
DB_NAME="baishali_test"
DB_USER="root"
#DB_PASS="your_password" # Replace with your actual password

# Create the posts table
mysql -u $DB_USER -e "CREATE DATABASE IF NOT EXISTS $DB_NAME;"
mysql -u $DB_USER -D $DB_NAME -e "
CREATE TABLE IF NOT EXISTS posts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    content TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);"

# Populate the posts table with entries for at least 10 users
for i in {1..10}
do
    mysql -u $DB_USER -D $DB_NAME -e "
    INSERT INTO posts (user_id, content) VALUES ($i, 'This is a post from user $i');"
done

echo "Posts table populated with entries for 10 users."