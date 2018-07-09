# "Team CE"
# Connor Hamilton
# Edwin Aguilera
# CptS 451, Spring 2018
# Milestone II
#########################################################

import json

insertsqlfile = 'populate.sql'

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('./Yelp-CptS451-2018/yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open(insertsqlfile, 'w')
        outfile.write("--business data\n")
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            print(line)
            data = json.loads(line)
            
            outfile.write("INSERT INTO Business (business_id, name, address, state, city, postal_code, latitude, longitude, stars, review_count, is_open) VALUES (");
            outfile.write("\'"+cleanStr4SQL(data['business_id'])+'\', ') #business id
            outfile.write("\'"+cleanStr4SQL(data['name']).encode('utf-8')+'\', ') #name
            outfile.write("\'"+cleanStr4SQL(data['address'])+'\', ') #full_address
            outfile.write("\'"+cleanStr4SQL(data['state'])+'\', ') #state
            outfile.write("\'"+cleanStr4SQL(data['city'])+'\', ') #city
            outfile.write("\'"+cleanStr4SQL(data['postal_code'])+'\', ')  #zipcode
            outfile.write(str(data['latitude'])+', ') #latitude
            outfile.write(str(data['longitude'])+', ') #longitude
            outfile.write(str(data['stars'])+', ') #stars
            outfile.write(str(data['review_count'])+', ') #reviewcount
            outfile.write(str(data['is_open'])) #openstatus           		
            outfile.write(');\n');
            
            for item in data['categories']:
            	outfile.write("INSERT INTO BusinessCategory (business_id, category) VALUES ")
            	outfile.write("(\'"+cleanStr4SQL(data['business_id'])+"\', \'"+cleanStr4SQL(item)+"\');")
            	outfile.write("\n")
            
            for item in data['hours']:
            	outfile.write("INSERT INTO BusinessHours (business_id, day, open, close) VALUES ")
            	day = item
            	duration = data['hours'][item].split("-")
            	outfile.write("(\'"+cleanStr4SQL(data['business_id'])+"\', \'"+day+"\', \'"+duration[0]+"\', \'"+duration[1]+"\');")
            	outfile.write("\n")

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():
    #write code to parse yelp_user.JSON
    with open('./Yelp-CptS451-2018/yelp_user.JSON', 'r') as f:
    	outfile = open(insertsqlfile, 'a')
    	outfile.write("--user data\n")
    	line = f.readline()
    	count_line = 0
    	while line:
    		print(line)
    		data = json.loads(line)
    		
    		outfile.write("INSERT INTO Users (user_id, average_stars, cool, fans, funny, name, review_count, useful, yelping_since) VALUES (")
    		outfile.write("\'"+cleanStr4SQL(data['user_id'])+'\', ')
    		outfile.write(str(data['average_stars'])+', ')
    		outfile.write(str(data['cool'])+', ')
    		outfile.write(str(data['fans'])+', ')
    		outfile.write(str(data['funny'])+', ')
    		outfile.write("\'"+cleanStr4SQL(data['name']).encode('utf-8')+'\', ')
    		outfile.write(str(data['review_count'])+', ')
    		outfile.write(str(data['useful'])+', ')
    		outfile.write("\'"+cleanStr4SQL(data['yelping_since'])+'\'')
    		outfile.write(');\n');
    		
    		line = f.readline()
    		count_line += 1
    	print(count_line)
    	outfile.close()
    	f.close()
    with open('./Yelp-CptS451-2018/yelp_user.JSON', 'r') as f:
    	outfile = open(insertsqlfile, 'a')
    	line = f.readline()
    	count_line = 0
    	while line:
    		print(line)
    		data = json.loads(line)
    		
    		for item in data['friends']:
    			outfile.write("INSERT INTO Friends (user_id, friend_id) VALUES ")
    			outfile.write("(\'"+cleanStr4SQL(data['user_id'])+"\', \'"+cleanStr4SQL(item)+"\');")
    			outfile.write("\n")
    		
    		line = f.readline()
    		count_line += 1
    	print(count_line)
    	outfile.close()
    	f.close()

def parseCheckinData():
    #write code to parse yelp_checkin.JSON
    with open('./Yelp-CptS451-2018/yelp_checkin.JSON', 'r') as f:
    	outfile = open(insertsqlfile, 'a')
    	outfile.write("--checkin data\n")
    	line = f.readline()
    	count_line = 0
    	while line:
    		print(line)
    		data = json.loads(line)
    		
    		time = []								# Create a list to contain the times
    		for item in data['time']:						# For each time in the set
    			values = data['time'][item]					# Get the time values into a list
    			partition = [0, 0, 0, 0]					# Create a list of time partitions [morning, afternoon, evening, night]
    			for v in values:						# For each of the values, determine its partition and increment the count
    				if v == "6:00" or v == "7:00" or v == "8:00" or v == "9:00" or v == "10:00" or v == "11:00":
    					partition[0] += 1
    				elif v == "12:00" or v == "13:00" or v == "14:00" or v == "15:00" or v == "16:00":
    					partition[1] += 1
    				elif v == "17:00" or v == "18:00" or v == "19:00" or v == "20:00" or v == "21:00" or v == "22:00":
    					partition[2] += 1
    				elif v == "23:00" or v == "0:00" or v == "1:00" or v == "2:00" or v == "3:00" or v == "4:00" or v == "5:00":
    					partition[3] += 1
    			time.append((item, partition[0], partition[1], partition[2], partition[3]))	# Add the result to the time list as a tuple
    		
    		for item in time:
    			outfile.write("INSERT INTO Checkin (business_id, day, morning, afternoon, evening, night) VALUES ")
    			outfile.write("(\'"+cleanStr4SQL(data['business_id'])+"\', \'"+cleanStr4SQL(item[0])+"\', "+str(item[1])+", "+str(item[2])+", "+str(item[3])+", "+str(item[4])+")")
    			outfile.write(';\n')
    		
    		line = f.readline()
    		count_line += 1
    	print(count_line)
    	outfile.close()
    	f.close()


def parseReviewData():
    #write code to parse yelp_review.JSON
    with open('./Yelp-CptS451-2018/yelp_review.JSON', 'r') as f:
    	outfile = open(insertsqlfile, 'a')
    	outfile.write("--review data\n")
    	line = f.readline()
    	count_line = 0
    	while line:
    		print(line)
    		data = json.loads(line)
    		
    		outfile.write("INSERT INTO Review (review_id, user_id, business_id, stars, created_on, review_text, useful, funny, cool) VALUES (")
    		outfile.write("\'"+cleanStr4SQL(data['review_id'])+'\', ')
    		outfile.write("\'"+cleanStr4SQL(data['user_id'])+'\', ')
    		outfile.write("\'"+cleanStr4SQL(data['business_id'])+'\', ')
    		outfile.write(str(data['stars'])+', ')
    		outfile.write("\'"+cleanStr4SQL(data['date'])+'\', ')
    		outfile.write("\'"+cleanStr4SQL(data['text']).encode('utf-8')+'\', ')
    		outfile.write(str(data['useful'])+', ')
    		outfile.write(str(data['funny'])+', ')
    		outfile.write(str(data['cool']))
    		outfile.write(');\n');
    		
    		line = f.readline()
    		count_line += 1
    	print(count_line)
    	outfile.close()
    	f.close()

parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
