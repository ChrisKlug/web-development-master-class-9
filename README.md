# Web Development Master Class

This repo contains the code used in my Web Development Master Class. 

## Getting started

To get started, you will need Visual Studio or VS Code, set up with the .NET Aspire workload. You will also need Docker Desktop installed, as it uses containers for a couple of the projects.

Before you can run the project, you need to do 2 things. Firs of all, you need to build a Docker image for the Identity Server project. This is quite eaisly done by running the following command in the terminal:

```bash
cd _resources/identity-server
docker build -t identity-server .
```

This creates a new `identity-server` Docker image that is used in the solution.

Next, you need to configure an SSL certificate for the Identity Server project. To do this, you need a certificate that you trust. The easiest is to simply use the one that ASP.NET Core sets up for you.

To get hold of the ASP.NET Core certificate you need to do the following steps:

1. Open Start Menu > Manage User Certificates
2. Navigate to Personal > Certificates
3. Find the certificate named `ASP.NET Core HTTPS development certificate`
4. Right-click the certificate and select `All Tasks > Export`
5. Click Next
6. Select `Yes, export the private key`
7. Click Next
7. Click Next
8. Add a password (the solution assumes __P@ssw0rd123!__)
9. Click Next
10. Save the file to /_rersources/ssl-cert.pfx

By doing this, the certificate will automatically be added to the Identity Server container and used by Kestrel.

__Note:__ The code for this is in [the AppHost's Program.cs file](/src/WebDevMasterClass/WebDevMasterClass.AppHost/Program.cs)

Once these steps have been completed, you should be able to simply press F5 to run the solution. As long as the __WebDevMasterClass.AppHost__ project is set as start-up project.

## Questions

If you have any questions, feel free to reach out to me. I'm available on Twitter...sorry...X...at [@zerokoll](https://twitter.com/zerokoll) and on LinkedIn at [https://www.linkedin.com/in/zerokoll/](https://www.linkedin.com/in/zerokoll/).

## Interested in the Workshop?

If you are interested in the workshop, please reach out to me on Twitter or LinkedIn. I would be more than happy to come and run the workshop for you and your team.