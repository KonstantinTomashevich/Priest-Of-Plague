using PriestOfPlague.Source.Ingame.UI;

namespace PriestOfPlague.Source.Ingame.Storyline
{
    public static class StorylineUtils
    {
        public static IStorylinePoint DialogPoint (DialogPanel dialogPanel, string text, string firstAnswer,
            string secondAnswer)
        {
            return new SimpleStorylinePoint (
                self =>
                {
                    dialogPanel.Show (
                        text, firstAnswer, secondAnswer,
                        () =>
                        {
                            self.ExitCode = 1;
                            self.ShouldExit = true;
                        },
                        () =>
                        {
                            self.ExitCode = 1;
                            self.ShouldExit = true;
                        });
                },
                self => { },
                self =>
                {
                    self.ShouldExit = false;
                    return self.ExitCode;
                });
        }

        public static IStorylinePoint TutorialPoint (DialogPanel dialogPanel, string tutorial)
        {
            return new SimpleStorylinePoint (
                self =>
                {
                    dialogPanel.Show (
                        tutorial, "Вперёд", "Назад",
                        () =>
                        {
                            self.ExitCode = 1;
                            self.ShouldExit = true;
                        },
                        () =>
                        {
                            self.ExitCode = -1;
                            self.ShouldExit = true;
                        });
                },
                self => { },
                self =>
                {
                    self.ShouldExit = false;
                    return self.ExitCode;
                });
        }
    }
}