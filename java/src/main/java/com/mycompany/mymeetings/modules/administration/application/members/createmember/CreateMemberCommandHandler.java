package com.mycompany.mymeetings.modules.administration.application.members.createmember;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandlerWithResult;
import com.mycompany.mymeetings.modules.administration.domain.members.Member;
import com.mycompany.mymeetings.modules.administration.domain.members.MemberRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.UUID;

@Service
public class CreateMemberCommandHandler
        implements CommandHandlerWithResult<CreateMemberCommand, UUID> {

    private final MemberRepository memberRepository;

    public CreateMemberCommandHandler(MemberRepository memberRepository) {
        this.memberRepository = memberRepository;
    }

    @Override
    @Transactional
    public UUID handle(CreateMemberCommand command) {
        Member member = Member.create(
                command.getMemberId(),
                command.getLogin(),
                command.getEmail(),
                command.getFirstName(),
                command.getLastName(),
                command.getName());

        memberRepository.save(member);

        return member.getId().getValue();
    }
}
