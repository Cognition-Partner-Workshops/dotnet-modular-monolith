package com.mycompany.mymeetings.modules.administration.application.contracts;

public interface CommandHandler<T extends Command> {

    void handle(T command);
}
