# ASP.NET Vue Skeleton
Minimalistic ASP.NET Vue Web App, see [Infestus WebApp](https://github.com/BerntA/InfestusWebApp) for a more in-depth version.

# Building

## Back-end
Navigate to server/, open server.sln if you are using Visual Studio.
Ensure that the app proj. is set as the startup proj.

### Database Migration (optional)
Set the database proj. as the active project, then run Add-Migration XYZ followed by Update-Database in the package console.

## Front-end
Navigate to client/

- Install node.JS > v17.9.1
- npm install
- 'npm run dev' or 'npm run build' for prod

# Troubleshooting
Ensure that the front-end points to the back-end url, open vite.config.js and verify the proxy setting.
