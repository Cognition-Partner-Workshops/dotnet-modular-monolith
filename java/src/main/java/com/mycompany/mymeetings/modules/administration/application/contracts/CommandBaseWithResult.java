package com.mycompany.mymeetings.modules.administration.application.contracts;

import java.util.UUID;

public abstract class CommandBaseWithResult<TResult> implements CommandWithResult<TResult> {

    private final UUID id;

    protected CommandBaseWithResult() {
        this.id = UUID.randomUUID();
    }

    protected CommandBaseWithResult(UUID id) {
        this.id = id;
    }

    @Override
    public UUID getId() {
        return id;
    }
}
