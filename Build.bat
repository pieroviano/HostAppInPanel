IF NOT DEFINED Configuration SET Configuration=Release
MSBuild.exe HostAppInPanel.sln -t:clean
MSBuild.exe HostAppInPanel.sln -t:restore -p:RestorePackagesConfig=true
MSBuild.exe HostAppInPanel.sln -m /property:Configuration=%Configuration%
git add -A
git commit -a --allow-empty-message -m ''
git push