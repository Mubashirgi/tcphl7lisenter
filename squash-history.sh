#!/bin/bash

# Script to squash all git history into a single clean commit

echo "⚠️  WARNING: This will rewrite git history permanently!"
echo "Make sure you have a backup before proceeding."
read -p "Continue? (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Aborted."
    exit 1
fi

echo "Squashing git history..."

# Step 1: Create orphan branch (no history)
git checkout --orphan clean-main

# Step 2: Add all current files
git add .

# Step 3: Create single initial commit
git commit -m "Initial release: HL7 TCP to HTTPS bridge service

Features:
- TCP to HTTPS message forwarding
- Windows service implementation  
- Configurable endpoints and authentication
- Event logging and error handling
- MSI installer included"

# Step 4: Delete old main/master branch
git branch -D master 2>/dev/null || git branch -D main 2>/dev/null || true

# Step 5: Rename current branch to main
git branch -m main

echo "History squashed successfully!"
echo "Next steps:"
echo "1. git remote set-url origin https://github.com/yourusername/NEW-REPO-NAME.git"
echo "2. git push -f -u origin main"
echo ""
echo "⚠️  Use 'git push -f' to force push the clean history"
