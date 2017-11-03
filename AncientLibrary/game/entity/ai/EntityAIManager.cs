using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai
{
    public class EntityAIManager
    {
        private List<EntityAI> tasks;

        private EntityAI currentTask;
        private bool executedTask;

        public EntityAIManager()
        {
            this.tasks = new List<EntityAI>();
            this.currentTask = null;
            this.executedTask = false;
        }

        public void AddTask(EntityAI task)
        {
            this.tasks.Add(task);
        }

        public void Update(GameTime gameTime)
        {
            if (tasks.Count > 0)
            {
                FindNextTask();
                UpdateCurrentTask(gameTime);
            }
        }

        private void FindNextTask()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if(tasks[i].ShouldExecute() && tasks[i] != currentTask)
                {
                    if (currentTask != null)
                    {
                        if (tasks[i].GetPriority() < currentTask.GetPriority())
                        {
                            currentTask.Stop();
                            currentTask = tasks[i];
                            executedTask = false;
                        }
                    }
                    else
                    {
                        currentTask = tasks[i];
                        executedTask = false;
                    }
                }
            }
        }

        private void UpdateCurrentTask(GameTime gameTime)
        {
            if(currentTask != null)
            {
                if (!executedTask)
                {
                   // Console.WriteLine("Executing: " + currentTask.GetType());
                    currentTask.Execute();
                    executedTask = true;
                }
                else
                {
                    if (currentTask.ShouldUpdate())
                        currentTask.Update(gameTime);
                    else
                    {
                        currentTask.Stop();
                        currentTask = null;
                    }
                }
            }
        }
    }
}
