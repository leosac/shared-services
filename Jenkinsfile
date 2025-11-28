pipeline {
    agent any
    environment {
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 1
    }
    tools {
        dotnetsdk 'dotnet8'
    }
	stages {
        stage('Pre-Build') {
            steps {
                dotnetRestore()
            }
        }
	    stage('Build') {
            steps {
                dotnetBuild()
            }
        }
        stage('Test') {
            steps {
                dotnetTest()
            }
        }
    }
}