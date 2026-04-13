package com.mycompany.mymeetings.modules.administration.application.contracts;

public interface CommandsScheduler {

    void enqueue(Command command);

    <TResult> void enqueue(CommandWithResult<TResult> command);
}
