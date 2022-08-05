## About the Application

- Technology Stack - C#.core, Google-Drive-API, Collection Framework

- This application will search for token given by you in your selected google drive .txt files and list with key value pair all the those files whose have the particular token in its content. Where Key as -- "File Name", and Value as -- "File ID". I mapped value as "File ID" because "WebViewLink" is not working right now though I have enabled all the scopes mentioned in the documentation.

- This app will take permission from users to authorize to get the AuthToken along with other credentials.

- In the first scope the app will autorize and after that it will print the folders and files with Name, MimeType, Id.

- Created a main function which will execute the functions/methods one by one.

- Implemented GetContent() metod which takes two argument of URL and AuthToken and returns the string which will contains the content of that particular file and also print the response JSON.

- Implemented SearchToken() metod which will take two argument one is list of key-value pair of filename and filecontent, and the second one is the token provided by the api for now I have hard coded it.
