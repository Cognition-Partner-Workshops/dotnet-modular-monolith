package com.mycompany.mymeetings.modules.administration.infrastructure;

import com.mycompany.mymeetings.modules.administration.domain.users.UserContext;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;
import org.springframework.stereotype.Component;

import java.util.UUID;

@Component
public class SpringSecurityUserContext implements UserContext {

    @Override
    public UserId getUserId() {
        // In a real implementation, this would extract the user ID from Spring Security context.
        // For now, return a placeholder that can be overridden via Spring Security configuration.
        return new UserId(UUID.fromString("00000000-0000-0000-0000-000000000001"));
    }
}
