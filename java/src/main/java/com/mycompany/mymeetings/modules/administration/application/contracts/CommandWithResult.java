package com.mycompany.mymeetings.modules.administration.application.contracts;

import java.util.UUID;

public interface CommandWithResult<TResult> {

    UUID getId();
}
