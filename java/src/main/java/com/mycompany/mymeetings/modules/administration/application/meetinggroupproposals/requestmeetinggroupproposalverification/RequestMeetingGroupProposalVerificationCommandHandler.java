package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.requestmeetinggroupproposalverification;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandlerWithResult;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupLocation;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposal;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposalRepository;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.UUID;

@Service
public class RequestMeetingGroupProposalVerificationCommandHandler
        implements CommandHandlerWithResult<RequestMeetingGroupProposalVerificationCommand, UUID> {

    private final MeetingGroupProposalRepository meetingGroupProposalRepository;

    public RequestMeetingGroupProposalVerificationCommandHandler(
            MeetingGroupProposalRepository meetingGroupProposalRepository) {
        this.meetingGroupProposalRepository = meetingGroupProposalRepository;
    }

    @Override
    @Transactional
    public UUID handle(RequestMeetingGroupProposalVerificationCommand command) {
        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                command.getMeetingGroupProposalId(),
                command.getName(),
                command.getDescription(),
                MeetingGroupLocation.create(command.getLocationCity(), command.getLocationCountryCode()),
                new UserId(command.getProposalUserId()),
                command.getProposalDate());

        meetingGroupProposalRepository.save(meetingGroupProposal);

        return meetingGroupProposal.getId().getValue();
    }
}
