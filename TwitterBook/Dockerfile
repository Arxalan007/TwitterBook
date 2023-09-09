FROM microsoft/dotnet:5.0.17

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY TwitterBook.csproj /build/

RUN dotnet restore ./build/TwitterBook.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./TwitterBook.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM microsoft/dotnet:5.0.17-aspnetcore-runtime
WORKDIR /app

COPY --from /build/out .

ENTRYPOINT ["dotnet", "TwitterBook.dll"]