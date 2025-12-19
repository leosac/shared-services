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
        stage('Sonar') {
            steps {
                withSonarQubeEnv('SonarQube Community') {
                    dotnetBuild()
                }
                timeout(time: 1, unit: 'HOURS') {
                    waitForQualityGate(abortPipeline: true)
                }
            }
            when {
                anyOf {
                    branch 'main'
                    buildingTag()
                    changeRequest()
                }
            }
        }
        stage('Test') {
            steps {
                dotnetTest()
            }
        }
    }
}