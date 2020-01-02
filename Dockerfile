FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ARG BUILDCONFIG=RELEASE

# Copy project files to avoid restoring packages if they haven't changed
COPY *.csproj ./build/
WORKDIR /build/
RUN dotnet restore

COPY . .
RUN dotnet publish -c $BUILDCONFIG -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build /build/out ./

# Install Cron, libgdiplus, cups
RUN apt-get update -qq && apt-get -y install cron libgdiplus cups -qq --force-yes

# Add export environment variable script and schedule
RUN echo "0 8 */2 * * root /app/autoPrint >> /var/log/cron.log 2>&1" > /etc/cron.d/schedule

# Configure 

# Create log file
RUN touch /var/log/cron.log
RUN chmod 0666 /var/log/cron.log

# Copy script
COPY start.sh start.sh

# Run Cron
CMD sh start.sh