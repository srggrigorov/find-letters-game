# find-letters-game

Версия Unity: 2019.4.28f1
 
В игре реализованы анимации с помощью плагина DoTween, также присутствуют UnityEvents.

В проекте расположен всего один префаб: cellPrefab, также в проекте находятся папки с текстовыми файлами и атласами справйтов.
Выбор набора данных осуществляется просто: в Объекте Quiz Manager нужно три параметра для каждого из типа данных (поля ответов, набор спрайтов, цель уровня/вопрос).
В поле для ответов помещается текстовый файл, в котором перечисленны ответы через пробел или с новой строки(регистр значения не имеет). Класс quizManager считывает данные с текстового файла и переводит в массив строк. В поле для спрайтов нужно поместить заранее заготовленный атлас спрайтов. Задание же можно написть в текстовом поле. Таким образом, игра легко расширяема, в нее без труда можно поместить новые данные без необходимости в работе программистов.
