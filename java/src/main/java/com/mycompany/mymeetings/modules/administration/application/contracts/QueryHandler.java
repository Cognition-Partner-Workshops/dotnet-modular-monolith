package com.mycompany.mymeetings.modules.administration.application.contracts;

public interface QueryHandler<T extends Query<TResult>, TResult> {

    TResult handle(T query);
}
