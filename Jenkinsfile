pipeline {
    agent any
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