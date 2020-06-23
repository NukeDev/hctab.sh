
# hctab.sh
[![Build Status](https://travis-ci.org/NukeDev/hctab.sh.svg?branch=master)](https://travis-ci.org/NukeDev/hctab.sh)
[![Nuget Version](https://img.shields.io/nuget/v/core.hctab.sh)](https://www.nuget.org/packages/core.hctab.sh)
[![Nuget](https://img.shields.io/nuget/dt/core.hctab.sh)](https://www.nuget.org/packages/core.hctab.sh)
[![GitHub issues](https://img.shields.io/github/issues/NukeDev/hctab.sh.svg)](https://github.com/NukeDev/CloudNoty/issues) 
[![GitHub stars](https://img.shields.io/github/stars/NukeDev/hctab.sh.svg)](https://github.com/NukeDev/CloudNoty/stargazers) 
[![GitHub license](https://img.shields.io/badge/license-AGPL-blue.svg)](https://raw.githubusercontent.com/NukeDev/hctab.sh/master/LICENSE)

This is a simple library, designed only for the .NET Framework, which allows the rapid creation of multi-step batch programs with scheduling included.
Through a simple configuration file (json for now, soon also through SQL database!) It is possible to configure the steps of the batch. The creation of a step occurs by extending the base class BatchStep and configuring the namespace relating to it in the json file. The activation and execution of the step takes place at runtime.

## Let's Start!

Before doing anything else you need to configure the environment.

At the moment it is possible to configure the batch through a configuration file "Config.json" which can be located in any folder. 

    class Program
    {
        static void Main(string[] args)
        {
            var batch = new Batch();
            batch.Init("Config/Config.json");
            Console.Read();
        }
    }
By instantiating the **Batch** class, you can call the **Init** method with the "ConfigPath" parameter which indicates the path where the configuration file is located.
## Config File

    {
	"Name":"Batch",
	"StepList": [
		{
			"ID": 0,
			"Name": "Step 1",
			"Description": "batch step 1",
			"Active": true,
			"ClassName": "test.hctab.sh.Step1",
			"Scheduling": [
				{
					"DaysOfWeek": "sun,mon,tue",
					"hhmmFrom": "15:30:00",
					"hhmmTo": "23:20:00"
				},
				{
					"DaysOfWeek": "sun,mon,tue",
					"hhmmFrom": "23:00:00",
					"hhmmTo": "24:00:00"
				}
			],
			"isSchedulerActive": true,
			"RunOrder": 1
		},
		{
			"ID": 1,
			"Name": "Load Files",
			"Description": "Load files from fb",
			"Active": true,
			"ClassName": "test.hctab.sh.LoadFiles",
			"Scheduling": [
				{
					"DaysOfWeek": "sun,mon,tue",
					"hhmmFrom": "15:30:00",
					"hhmmTo": "23:00:00"
				}
			],
			"isSchedulerActive": true,
			"RunOrder": 2
		}
	
	]
	}
This is the configuration file also used in the Test project, test.hctab.sh.

- "**Name**": *String type value*, used to declare the batch name
- "**StepList**": *Array type value*, contains a list of steps
- "**ID**": *Int type value*, identifies the step
- "**Name**": *String type value*, used to declare the step name
- "**Description**": *String type value*, used to declare the description of the step 
- "**Active**": *Boolean type value*, used to define if the step is active or not
- "**ClassName**": *String type value*, used to specify the class name (with namespace) of the step. Very important for runtime activation.
- "**Scheduling**": *Array type value*, contains a list of schedulers
- "**DaysOfWeek**": *String type value*, it contains the days (the first 3 letters) separated by commas, used for the activation of the scheduling of the step
- "**hhmmFrom**": *String type value*, contains the start time (00:00:00) in which the step can be scheduled
- "**hhmmTo**": *String type value*, contains the end time (00:00:00) in which the step can be scheduled
- "**isSchedulerActive**": *Boolean type value*, if it is true, it will enable the schedulations, else it will check for the "**Active**" parameter to determine if the step can be ran.
- "**RunOrder**": *Int type value*, to determine the order of the step, ascending order.

## Step Code Example

    using core.hctab.sh.Batch;
	using core.hctab.sh.Interfaces;
	using System;
	using System.Collections.Generic;


	namespace test.hctab.sh
	{
	    class Step1 : BatchStep, IBatchStep
	    {

	        public override bool IsApplicable()
	        {
	            return true;
	        }

	        public override void ReadData()
	        {
	        }

	        public override void SaveData()
	        {
	        }

	        public override void Verify()
	        {
	        }
	    }
	   
	}


For any more information check the Test project!
