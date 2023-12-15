## Notes :

If you encounter DLLNotFound exception about EmguCV , apply these settings : 

1. **Uninstall all NuGet Packages** [This removes the package.config file]
2. Go to Tools --> Options --> NuGet Package Manager --> General
3. Change the default **package management format to "PackageReference"**
4. Check "allow format selection on first package install"
5. Click OK
6. Install Emgu.CV, Emgu.CV.Bitmap, Emgu.CV.UI and Emgu.CV.runtime.windows