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
            environment {
                scannerHome = tool 'SonarScanner for MSBuild'
            }
            steps {
                withSonarQubeEnv('SonarQube Community') {
                    sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:'leosac_shared-services_c6e04130-a79b-4220-8539-6115f367bcd4'"
                    dotnetBuild()
                    sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
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