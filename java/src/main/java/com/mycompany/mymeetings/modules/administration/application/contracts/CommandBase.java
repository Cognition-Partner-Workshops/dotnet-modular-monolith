package com.mycompany.mymeetings.modules.administration.application.contracts;

import java.util.UUID;

public abstract class CommandBase implements Command {

    private final UUID id;

    protected CommandBase() {
        this.id = UUID.randomUUID();
    }

    protected CommandBase(UUID id) {
        this.id = id;
    }

    @Override
    public UUID getId() {
        return id;
    }
}
