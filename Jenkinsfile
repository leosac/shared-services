pipeline {
    agent any
    tools {
        dotnet 'dotnet8'
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