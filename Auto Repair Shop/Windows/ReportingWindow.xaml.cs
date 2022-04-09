﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using Auto_Repair_Shop.Classes;
using Auto_Repair_Shop.Entities;

namespace Auto_Repair_Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportingWindow.xaml.
    /// </summary>
    public partial class ReportingWindow : Window
    {
        /// <summary>
        /// Название шрифта, который будет использоваться для генерации документа.
        /// </summary>
        public string fontFamily { get; set; } = "Georgia";

        /// <summary>
        /// Место, где будет сохранен документ.
        /// </summary>
        public string folderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public ReportingWindow()
        {
            InitializeComponent();
            initializeHeaders();
            DataContext = this;

            setFont.ToolTip = $"Текущий выбранный шрифт: {fontFamily}.";
            legacyDocumentType.ToolTip = "Если выбрано, программа будет формировать отчёт в устаревших форматах документов.";
            setFolder.ToolTip = "Позволяет установить директорию, в которой будет сохранен документ с отчётом.";
        }

        /// <summary>
        /// Заполняет заголовки текстом.
        /// </summary>
        private void initializeHeaders() {
            mainHeader.Text = "Приветствуем вас в мастере формирования отчётности. Это окно поможет вам легко сформировать нужный документ.\n" +
                              "Для работы вам будет нужно указать место в которое будет сохранен документ (по умолчанию такой директорией является рабочий стол).\n" +
                              "Также можно указать дополнительные параметры: шрифт документа и тип используемого расширения.";

            secondHeader.Text = "После формирования отчёта вы сможете открыть его и просмотреть всю доступную информацию по текущим заказам.\n" +
                                "Заметьте, что если в настройках отключено отображение старых заказов, то и в отчётности они не появятся.";
        }

        private void setFont_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FontDialog();
            var result = dialog.ShowDialog();

            if (System.Windows.Forms.DialogResult.OK == result) {
                fontFamily = dialog.Font.Name;

                setFont.ToolTip = $"Текущий выбранный шрифт: {fontFamily}.";
            }
        }

        private void setFolder_Click(object sender, RoutedEventArgs e) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Выбор директории";

            var result = dialog.ShowDialog();

            if (System.Windows.Forms.DialogResult.OK == result) {
                folderPath = dialog.SelectedPath;
                currentFolder.Text = folderPath;

                MessageBox.Show("Обратите внимание, что в случае наличия в директории файла со схожим именем, он будет перезаписан.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void generateExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Проверяет состояние полей настроек и начинает генерацию отчёта Word.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void generateWord_Click(object sender, RoutedEventArgs e)
        {
            if (legacyDocumentType.IsChecked.HasValue && legacyDocumentType.IsChecked.Value) {
                var confirm = MessageBox.Show("Формирование документа Word в устаревшем формате на данный момент недоступно.\n\n" +
                                              "Использовать современный формат (*.docx)?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes) {
                    beginWordGeneration();
                }

            } else {
                beginWordGeneration();
            }
        }

        /// <summary>
        /// Проводит генерацию отчётности в Word.
        /// </summary>
        private void beginWordGeneration() {
            string path = Path.Combine(folderPath, "Отчёт.docx");
            WordReporting reportGenerator = new WordReporting(path, fontFamily, false, DBEntities.Instance.Service_Request.ToList());

            var result = reportGenerator.generateReport();

            if (result) {
                MessageBox.Show("Отчёт успешно создан.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            } else {
                MessageBox.Show("Произошла ошибки во время создания отчёта.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
