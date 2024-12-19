# Simple Threading Example
## Long running task with multiple sub tasks
This is a common thread that keeps appearing when discussing threading with developers who rarely need to understand or use threading.  I understand why, understanding what effects threading pools have, when to start up a new pool, or pull a thread from an exisitng pool, can throw up a few questions.

Therefore, this is based on a very simple console app with a while(true) loop that only ends once all tasks have completed.

These very simple examples do not yet explore the use of Parallelism nor disposal of objects. 
If you want to know the difference between using Cancellation tokens, and values to exit loops, then run the MixedSimpleThread.  Task A should cancel at 100

![threading](https://github.com/user-attachments/assets/34d34f63-9305-47d3-94b3-b097bee61f91)
