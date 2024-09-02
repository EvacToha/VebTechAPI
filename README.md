# Ключевые моменты:
- Было написано простое ASP.NET Core Web Api, следующее Onion Layers структуре.
- Использован принцип CQRS, разделяющий операции записи и операции чтения на отдельные модули. Данный принцип реализован с помощью NuGet пакета MediatR.
- Для регистрирования сервисов был использован NuGet пакет Autofac, использующийся для внедрения зависимостей(Dependency Injection). Также, в структуре проекта соблюдается принцип Dependency Inversion принципа SOLID: взаимодействие между модулями происходит посредством абстракций, и модули, которые имеют высший приоритет в бизнес логике приложения, не зависят от менее приоритетных.
- В качестве ORM была выбрана Entitity Framework Core, а также Code First подход.
- Реализована инициализация таблицы ролей начальными данными.
- Для валидации запросов используется Fluent Validation. Жизненный цикл валидации осуществлен посредством реализации интерфейса IPipelineBehavior.
- Также в конвеер обработки запроса было добавлено Exceptions Handling Middleware, отвечающее за обработку ошибок всего жизненного цикла запроса.
- Написан функционал поддерживающий сортировку и фильтрацию по нескольким полям одновременно, выполненный на уровне запроса к базе данных. А также валидируются методы фильтрации, применяемые к полям представляющие различные типы данных.
- Использован Swagger для документирования входных и выходных контрактов запросов. Также документируются возможные ошибки запросов. 
