package com.mycompany.mymeetings.modules.administration.application.contracts;

public interface AdministrationModule {

    void executeCommand(Command command);

    <TResult> TResult executeCommand(CommandWithResult<TResult> command);

    <TResult> TResult executeQuery(Query<TResult> query);
}
