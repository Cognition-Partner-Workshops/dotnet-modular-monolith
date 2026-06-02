package com.mycompany.mymeetings.modules.administration.application.members.createmember;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandBaseWithResult;

import java.util.UUID;

public class CreateMemberCommand extends CommandBaseWithResult<UUID> {

    private final UUID memberId;
    private final String login;
    private final String email;
    private final String firstName;
    private final String lastName;
    private final String name;

    public CreateMemberCommand(UUID id, UUID memberId, String login, String email,
                                String firstName, String lastName, String name) {
        super(id);
        this.memberId = memberId;
        this.login = login;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.name = name;
    }

    public UUID getMemberId() { return memberId; }
    public String getLogin() { return login; }
    public String getEmail() { return email; }
    public String getFirstName() { return firstName; }
    public String getLastName() { return lastName; }
    public String getName() { return name; }
}
