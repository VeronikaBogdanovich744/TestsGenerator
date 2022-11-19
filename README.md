# TestsGenerator
https://bsuir.ishimko.me/mpp-dotnet/4-tests-generator
Необходимо реализовать многопоточный генератор шаблонного кода тестовых классов для одной из библиотек для тестирования (NUnit, xUnit, MSTest) по тестируемым классам.
Входные данные 
Список файлов, для классов из которых необходимо сгенерировать тестовые классы.
Путь к папке для записи созданных файлов.
Ограничения на секции конвейера (см. далее).
Выходные данные
Файлы с тестовыми классами: в каждом выходном файле должен быть только один тестовый класс, соответствующий одному тестируемому классу, вне зависимости от того, как были расположены тестируемые классы в исходных файлах. 
Например: Input.cs (с классами Foo и Bar) -> FooTests.cs, BarTests.cs.
Все сгенерированные тестовые классы должны компилироваться при включении в отдельный проект, в котором имеется ссылка на проект с тестируемыми классами.
Все сгенерированные тесты должны завершаться с ошибкой.
Схема работы
Генерация должна выполняться в конвейерном режиме "производитель-потребитель" и состоять из трех этапов: 
параллельная загрузка исходных текстов в память (с ограничением количества файлов, загружаемых за раз);
генерация тестовых классов в многопоточном режиме (с ограничением максимального количества одновременно обрабатываемых задач); 
параллельная запись результатов на диск (с ограничением количества одновременно записываемых файлов).
При реализации использовать async/await и асинхронный API. Для реализации конвейера использовать Dataflow API. Полезные ссылки:
Dataflow (Task Parallel Library)
Walkthrough: Creating a Dataflow Pipeline
Главный метод генератора должен возвращать Task и не выполнять никаких ожиданий внутри (блокирующих вызовов task.Wait(), task.Result, etc). Для ввода-вывода также необходимо использовать асинхронный API (см. Asynchronous File I/O).
Необходимо сгенерировать по одному пустому тесту на каждый публичный метод тестируемого класса. Следует учитывать перегрузку методов (несколько методов с одинаковыми именами, но разными параметрами).