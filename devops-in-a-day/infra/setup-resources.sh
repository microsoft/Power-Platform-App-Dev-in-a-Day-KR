#!/bin/bash

set -e

# Invoke GitHub Actions workflow_dispatch API
dispatched=$(curl -H "Accept: application/vnd.github+json" \
    -H "User-Agent: Autopilot" \
    -H "Authorization: token $GH_ACCESS_TOKEN" \
    https://api.github.com/repos/$GH_USERNAME/$GH_REPOSITORY_NAME/actions/workflows/azure-dev.yml/dispatches \
    -d "{ \"ref\": \"$GH_BRANCH_NAME\" }")
    # -d "{ \"ref\": \"$GH_BRANCH_NAME\", \"inputs\": { \"azure_env_name\": \"$AZURE_ENV_NAME\", \"azure_location\": \"$AZURE_LOCATION\", \"azure_subscription_id\": \"$AZURE_SUBSCRIPTION_ID\" } }")
