1. Creating a Pool --
	- Create an empty GameObject
	- Add PoolInitializer script to said object
	- On the PoolInitializer set the number of preloaded object types to generate
	- For each Initializer Object
		- Base Pool Count: the number of preloaded objects
		- Base Object: object being instantiated
		- Object Tag Line: string used in the dictionary hash function to distinguish object types

2. How to set up Base object for Pool to use --
	- Add the ResourcePoolID script to the prefab object
	- Set the Resource ID to be the same as the Object Tag Line used in the hash of the Pool

3. Using Pool in code --
	- Find the Pool object created in step 1
	- Access that object's ObjectPool
	- Obtaining an object to use:
		- GetObject("ID from hash");
	- After you are finished with an object
		- KillObject(GameObject);