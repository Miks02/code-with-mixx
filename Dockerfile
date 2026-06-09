FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish ./

COPY --from=build --chown=app:app /app/publish ./

USER app

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000
CMD ["dotnet", "CodeWithMixx.dll", "--urls", "http://+:${PORT:-10000}"