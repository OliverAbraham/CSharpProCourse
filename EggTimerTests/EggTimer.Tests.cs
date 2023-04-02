using EggTimerViewModel;
using FluentAssertions;

namespace EggTimerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void When_user_starts_the_app_the_time_preselection_should_be_5_seconds()
        {
            var sut = CreateSut();
            sut.CurrentTime.Should().Be("00:05");
        }

        [TestMethod]
        public void When_user_starts_the_app_the_timer_should_not_run()
        {
            var sut = CreateSut();
            sut.IsRunning.Should().BeFalse();
        }

        [TestMethod]
        public void Startbutton_should_initially_show_start()
        {
            var sut = CreateSut();
            sut.StartButtonContent.Should().Be("Start");
        }

        [TestMethod]
        public void Startbutton_should_initially_be_green()
        {
            var sut = CreateSut();
            sut.StartButtonBackground.Should().Be("#FF90EE90");
        }

        [TestMethod]
        public void When_user_presses_3min_button_preselection_should_be_3_minutes()
        {
            var sut = CreateSut();
            sut.PreselectTime("3");
            sut.CurrentTime.Should().Be("03:00");
        }

        [TestMethod]
        public void When_user_presses_the_start_button_timer_should_be_started()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.IsRunning.Should().BeTrue();
        }

        [TestMethod]
        public void When_user_presses_the_start_button_again_timer_should_be_stopped()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.IsRunning.Should().BeTrue();

            sut.Button_Start_Click();
            sut.IsRunning.Should().BeFalse();
        }

        [TestMethod]
        public void When_user_presses_the_start_button_it_should_turn_to_red()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.StartButtonBackground.Should().Be("#FFFFA07A");
        }

        [TestMethod]
        public void When_user_presses_the_start_button_again_it_should_turn_to_green_again()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.Button_Start_Click();
            sut.StartButtonBackground.Should().Be("#FF90EE90");
        }

        [TestMethod]
        public void When_user_presses_the_start_button_it_should_show_the_word_stop()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.StartButtonContent.Should().Be("Stop");
        }

        [TestMethod]
        public void When_user_presses_the_start_button_again_it_should_show_start_again()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.Button_Start_Click();
            sut.StartButtonContent.Should().Be("Start");
        }

        [TestMethod]
        public void When_the_timer_runs_to_zero_it_should_stop_automatically()
        {
            var sut = CreateSut();
            sut.Button_Start_Click();
            sut.IsRunning.Should().BeTrue();

            sut.Count();
            sut.Count();
            sut.Count();
            sut.Count();
            sut.Count();
            sut.IsRunning.Should().BeFalse();
            sut.StartButtonContent.Should().Be("Start");
            sut.StartButtonBackground.Should().Be("#FF90EE90");
        }
 
        private static ViewModel CreateSut()
        {
            var sut = new ViewModel();
            sut.StartTimer = delegate () { };
            sut.StopTimer = delegate () { };
            sut.PlaySound = delegate () { };
            return sut;
        }
   }
}