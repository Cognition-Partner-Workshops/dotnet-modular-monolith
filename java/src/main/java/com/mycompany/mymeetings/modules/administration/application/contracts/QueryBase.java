package com.mycompany.mymeetings.modules.administration.application.contracts;

import java.util.UUID;

public abstract class QueryBase<TResult> implements Query<TResult> {

    private final UUID id;

    protected QueryBase() {
        this.id = UUID.randomUUID();
    }

    protected QueryBase(UUID id) {
        this.id = id;
    }

    @Override
    public UUID getId() {
        return id;
    }
}
