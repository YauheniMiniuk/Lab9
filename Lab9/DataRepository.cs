using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9
{
    public interface IDataRepository
    {
        IEnumerable<Player> Players { get; }
        IEnumerable<Team> GetTeams();
        Task<Player> GetPlayer(int id);
        void CreatePlayer(Player player, DataGridView dataGridView);
        Task CreatePlayerAsync(Player player, DataGridView dataGridView);
        void CreateTeam (Team team);
        Task CreateTeamAsync(Team team);
        void UpdatePlayer(Player player, DataGridView dataGridView);
        Task UpdatePlayerAsync(Player player, DataGridView dataGridView);
        void DeletePlayer(int id, DataGridView dataGridView);
        Task DeletePlayerAsync(int id, DataGridView dataGridView);
    }
    public class DataRepository : IDataRepository
    {
        SoccerContext context;
        public DataRepository()
        {
            context = new SoccerContext();
        }
        public IEnumerable<Player> Players => context.Players;
        public IEnumerable<Team> GetTeams()
        {
            return context.Teams;
        }
        public async Task<Player> GetPlayer(int id)
        {
            return await context.Players.FindAsync(id);
        }
        public async void CreatePlayer(Player player, DataGridView dataGridView)
        {
            context.Players.Add(player);
            Thread.Sleep(20000);
            await context.SaveChangesAsync();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.DataSource = context.Players.ToList();
            });
            //dataGridView.DataSource = Players.ToList();
        }
        public async Task CreatePlayerAsync(Player player, DataGridView dataGridView)
        {
            await Task.Run(() => CreatePlayer(player, dataGridView));
        }
        public async void CreateTeam(Team team)
        {
            Thread.Sleep(20000);
            context.Teams.Add(team);
            await context.SaveChangesAsync();
        }
        public async Task CreateTeamAsync(Team team)
        {
            await Task.Run(() => CreateTeam(team));
        }
        public async void UpdatePlayer(Player player, DataGridView dataGridView)
        {
            var dbEntry = GetPlayer(player.Id).Result;
            if (dbEntry != null)
            {
                dbEntry.Name = player.Name;
                dbEntry.Age = player.Age;
                dbEntry.Teams = dbEntry.Teams.ToList();
                await context.SaveChangesAsync();
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.DataSource = context.Players.ToList();
                });
            }
        }
        public async Task UpdatePlayerAsync(Player player, DataGridView dataGridView)
        {
            await Task.Run(() => UpdatePlayer(player, dataGridView));
        }
        public async void DeletePlayer(int id, DataGridView dataGridView)
        {
            Thread.Sleep(3000);
            var player = GetPlayer(id).Result;
            if (player != null)
            {
                context.Players.Remove(player);
                await context.SaveChangesAsync();
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.DataSource = context.Players.ToList();
                });
            }
        }
        public async Task DeletePlayerAsync(int id, DataGridView dataGridView)
        {
            await Task.Run(() => DeletePlayer(id, dataGridView));
        }
    }
}
