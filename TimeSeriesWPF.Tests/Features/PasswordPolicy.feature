Feature: Проверка надёжности пароля
  Как администратор системы
  Я хочу устанавливать требования к надёжности пароля
  Чтобы обеспечить безопасность учётных записей пользователей

  @validation
  Scenario Outline: Проверка политики надёжности пароля
    Given система проверки пароля активна
    When пользователь вводит пароль "<password>"
    Then система должна вернуть результат "<result>"
    
    Examples:
      | password      | result  |
      | short         | false   |
      | abcdefgh      | false   |
      | StrongPass1   | true    |
      | Pass1234      | true    |
      | null          | false   |
      |               | false   |