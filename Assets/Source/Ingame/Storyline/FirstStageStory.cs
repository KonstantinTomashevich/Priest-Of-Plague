using System.Collections;
using System.Collections.Generic;
using PriestOfPlague.Source.Ingame.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PriestOfPlague.Source.Ingame.Storyline
{
    public class FirstStageStory : MonoBehaviour
    {
        public DialogPanel DialogPanelRef;
        public GameObject UndeadPrefab;
        public GameObject GamisoniaPrefab;

        private Storyline _storyline;
        private SphereCollider _collider;

        private void Start ()
        {
            _storyline = new Storyline ();
            _collider = GetComponent <SphereCollider> ();

            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef,
                "Бобро пожаловадь в игру \"Жрец Microsoft Access\". Да здравствует обучение!", "Да", "Lf"));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "На панели слева находятся счётчики здоровья и выносливости. Также там будут отображаться" +
                " эффекты, наложенные на персонажа."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Обожрётесь и отравитесь -- панель слева уведомит вас о результате вашего поведения."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Справа панель инвентаря. Она может быть скрыта и показана " +
                "вновь нажатием кнопки \"Инвентарь П/С\""));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Чтобы выбрать предмет в инвентаре и посмотреть" +
                " информацию о нём, нажмите на его иконку."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Снижу панель действий. Нажатие на значок джойстика отменяет" +
                " выполнение текущего действия (кроме ходьбы)."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Каждое действие (кроме ходьбы) затрачивает выносливость."));
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Также каждое действие затрачивает заряд необходимого для действия предмета."));
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Ещё каждое действие требует время на подготовку. " +
                "Ну и предмет нужно не забыть выбрать в инвентаре."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Областное действие -- воздействует всех юнитов в секторе круга радиусом R" +
                " с углом A в обе стороны от лица персонажа."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Еденичное действие -- воздействует на ближайшего юнита в секторе круга радиусом R" +
                " с углом A в обе стороны от лица персонажа."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Целовое действие -- воздействует на выбранного юнита. Юнит выбирается кликом по нему."));
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Стена огня. Областное. Огненный посох. R=5+У. A=45+У. Наносит урон и поджигает врага."));
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Выпить зелье. Уникальное. Игрок выпивает или съедает указанный" +
                " предмет в инвентаре (если это возможно)."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Быстрое лечение. Целевое. Сфера лечения. Восстанавливает здоровье мгновенно."));
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Продолжительное лечение. Целевое. Сфера лечения. Добавляет состояние \"лечение\"," +
                " восстанавливающие ОЗ со временем."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Украсть ОЗ. Целевое. Сфера некромантии. Наносит урон юниту" +
                " и восстанавливает столько же здоровья игроку."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Огненный шар. Еденичное. Огненный посох." +
                " R=10+3У. A=10. Наносит урон и поджигает врага."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef, "Молния. Еденичное. Посох молний." +
                " R=10+5У. A=10. Наносит урон и парализует врага на 5У секунд."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef, "Электрозащита. Посох молний." +
                " Целевое. Делает цель неуязвимой для внешнего урона на 5У секунд."));
            
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef, "Воскресить. Сфера некромантии." +
                 " Целевое. Воскрешает указанного юнита на вашей стороне, даёт ему 10*У% ОЗ."));
            
            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef, "Отравить. Отравляющий посох." +
                " Целевое. Вызывает у цели отравление на 30У секунд."));

            _storyline.Points.Add (StorylineUtils.TutorialPoint (DialogPanelRef,
                "Для движения вперёд жмите W, для поворота A и D. Удачной игры!"));
            
            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef, "Сергей Иванович, тут одно дело...",
                "Пошли на <censored>, я иду есть!", "Потом, я занят!"));
            
            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef, "Но это срочно.",
                "Тогда вам переделка со штрафом.", "Куда вы спешите как голый в баню!"));
            
            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef,
                "ОНИ из деканата! (появляется 5 пересдающих)",
                "Переделка с двумя штрафами!", "Пошли на <censored>!"));
            
            _storyline.Points.Add (new SpawnAndKillStorylinePoint (UndeadPrefab, _collider, 5, 2));
            
            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef,
                "Похоже, это ещё не все...", "Куда вы спешите, как голые в баню?", "Нули поставлю!"));
            
            _storyline.Points.Add (new SpawnAndKillStorylinePoint (UndeadPrefab, _collider, 5, 2));
            
            _storyline.Points.Add (StorylineUtils.DialogPoint (DialogPanelRef,
                "Мы думали, что он отчислился, но оказалось, что не полностью…",
                "И не таких <censored>!", "Сейчас всё исправим, болезненным путём."));
            
            _storyline.Points.Add (new SpawnAndKillStorylinePoint (GamisoniaPrefab, _collider, 1, 2));
            _storyline.Points.Add (new SimpleStorylinePoint (
                self =>
                {
                    DialogPanelRef.Show (
                        "Таки оно отчислилось! Акт пройден!", "В меню!", "Остаться пока тут.",
                        () =>
                        {
                            self.ExitCode = 1;
                            self.ShouldExit = true;
                        },
                        () =>
                        {
                            self.ShouldExit = false;
                        });
                },
                self => { },
                self =>
                {
                    self.ShouldExit = false;
                    return self.ExitCode;
                }));
            
            _storyline.Points.Add (new SimpleStorylinePoint (
                self => SceneManager.LoadScene (0),
                self => { },
                self => 0));
        }

        private void Update ()
        {
            _storyline.Update ();
        }
    }
}