@FOR %%I in ("*.sln") DO @(
    @echo %%I
	call dotnet format "%%I" --exclude Library Assets/Plugins Packages
)
@pause