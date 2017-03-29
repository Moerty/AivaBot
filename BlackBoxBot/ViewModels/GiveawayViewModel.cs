﻿using BlackBoxBot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BlackBoxBot.ViewModels
{
    [PropertyChanged.ImplementPropertyChanged]
    public class GiveawayViewModel
    {
        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand RollCommand { get; set; } = new RoutedCommand();

        public DispatcherTimer TimerToEnd { get; set; }

        public bool Mod { get; set; } = true;
        public bool Sub { get; set; } = true;
        public bool Admin { get; set; } = true;
        public bool User { get; set; } = true;
        public int SubLuck { get; set; } = 1;
        public string Keyword { get; set; }
        public string Winner { get; set; }

        public bool UncheckWinner { get; set; } = false;

        public bool IsStarted { get; set; } = false;

        public Giveaway.Giveaway GiveawayInstance { get; set; }
        public ObservableCollection<string> Winners { get; set; }
        public Models.GiveawayModel Model { get; set; }

        public GiveawayViewModel()
        {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartCommand, Start));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StopCommand, Stop));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(RollCommand, Roll));

            // Create Models
            CreateModels();
            

            TimerToEnd = new DispatcherTimer(DispatcherPriority.Normal);
            TimerToEnd.Interval = TimeSpan.FromMinutes(3);
            TimerToEnd.Tick += TimerToEnd_Tick;
        }

        private void TimerToEnd_Tick(object sender, EventArgs e)
        {
            TimerToEnd.Stop();
            Stop();
        }

        private void Start(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Keyword)) return;

            var Model = new Giveaway.Models.GiveawayModel
            {
                Admin = Admin,
                Mod = Mod,
                Sub = Sub,
                User = User,
                SubLuck = SubLuck,
                Keyword = Keyword,
            };

            Winners = new ObservableCollection<string>();
            GiveawayInstance = new Giveaway.Giveaway(Model);

            IsStarted = true;
            TimerToEnd.Start();
        }

        private void Roll(object sender, EventArgs e)
        {
            var raffleWinner = GiveawayInstance.UserList.GetWinner();

            if(UncheckWinner)
                if(Winners.SingleOrDefault(x => String.Compare(raffleWinner.Username, x, StringComparison.OrdinalIgnoreCase) == 0) == null)
                    Winners.Add(raffleWinner.Username);
        }

        private void Stop(object sender = null, EventArgs e = null)
        {
            IsStarted = false;

            GiveawayInstance.StopGiveawayRegistration();
        }


        private void CreateModels()
        {
            Model = new Models.GiveawayModel();
            
            Model.Text = new Models.GiveawayModel.TextModel
            {
                StatusActive = Config.Language.Instance.GetString("GiveawayStatusActive"),
                StatusInactive = Config.Language.Instance.GetString("GiveawayStatusInactive"),
                ButtonGiveawayRoll = Config.Language.Instance.GetString("GiveawayButtonGiveawayRoll"),
                ButtonGiveawayStart = Config.Language.Instance.GetString("GiveawayButtonGiveawayStart"),
                ButtonGiveawayStop = Config.Language.Instance.GetString("GiveawayButtonGiveawayStop"),
                SubLuckText = Config.Language.Instance.GetString("GiveawaySubLuckText"),
                CommandWatermark = Config.Language.Instance.GetString("GiveawayCommandWatermark"),
                ExpanderUsersDescription = Config.Language.Instance.GetString("GiveawayExpanderUsersDescription"),
                ExpanderWinnersDescription = Config.Language.Instance.GetString("GiveawayExpanderWinnersDescription"),
                CheckBoxAdmin = Config.Language.Instance.GetString("GiveawayCheckBoxAdmin"),
                CheckBoxMod = Config.Language.Instance.GetString("GiveawayCheckBoxMod"),
                CheckBoxSub = Config.Language.Instance.GetString("GiveawayCheckBoxSub"),
                CheckBoxViewer = Config.Language.Instance.GetString("GiveawayCheckBoxViewer"),
                UncheckWinner = Config.Language.Instance.GetString("GiveawayUncheckWinner"),
            };
        }
    }
}