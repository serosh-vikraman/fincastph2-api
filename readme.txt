How to run application locally
----------------------------------------------------------
1. Open the TechnipFMC.Finapp.Service.API.sln solution. Run the solution,  API url(eg http://localhost:6300)
2. Open the APP code inside the folder Finapp in VS Code
            You need to change the baseUrl value in config.txt file  You can find the file in /src/assets/configuration folder.
            Replace the baseUrl value with new api URL http://localhost:6300/
3. Change withCredential: false to withCredential: true in  src\environments\environment.ts file
4. Run application using run command from the terminal npm run ng serve -o

Deployment Steps
------------------------------------------------------------
1. Publish API
2. Take production build of Angular app using the command =>  ng build --prod
3. Copy both the API publish file and App build files in same folder 
4. Changes in  API web.config file
        a. Change Connection string (FinappConnection)
        b. Change Appsettings URL_Production key. eg <add key="URL_Production" value="http://AE001VM0643/finapp" />
5. Change the baseUrl value in config.txt file  Please find the file in /assets/configuration folder. eg. http://AE001VM0643/finapp
6. Access the application (http://AE001VM0643/finapp)