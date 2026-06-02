package com.mycompany.mymeetings.modules.administration.application.contracts;

public interface CommandHandlerWithResult<T extends CommandWithResult<TResult>, TResult> {

    TResult handle(T command);
}
