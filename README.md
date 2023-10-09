# PubSubExercise
Small solution that demonstrates the concepts of the Publish/Subscribe pattern. 

## Requirements
Produce a small solution that demonstrates the concepts of the Publish/Subscribe pattern performing the followings:
- Take an input of data
- Transform that data in some way
- Transport that data to a set of subscribers
- Have the subscribers display the transformed data

## Publish/Subscribe Pattern
### Pattern definition
The Publish/Subscribe (pub/sub) pattern is an architectural design pattern that provides a framework for exchanging messages between publishers and subscribers, without coupling the senders to the receivers.

__Use this pattern when:__
- An application needs to broadcast information to a significant number of consumers.
- An application needs to communicate with one or more independently-developed applications or services, which may use different platforms, programming languages, and communication protocols.
- An application can send information to consumers without requiring real-time responses from the consumers.
- The systems being integrated are designed to support an eventual consistency model for their data.
- An application needs to communicate information to multiple consumers, which may have different availability requirements or uptime schedules than the sender.

__This pattern might not be useful when:__
- An application has only a few consumers who need significantly different information from the producing application.
- An application requires near real-time interaction with consumers.

### Pattern advantages/disadvantages
Advantages
- __Low coupling__. It decouples applications that still need to communicate. Applications can be managed independently, and messages can be properly managed even if one or more receivers are offline.
- __Scalability__. It increases scalability and improves responsiveness of the sender. The sender can quickly send a single message to the input channel, then return to its core processing responsibilities. The messaging infrastructure is responsible for ensuring messages are delivered to interested subscribers.
- __Reliability__ and asynchronous workflows. Asynchronous messaging helps applications continue to run smoothly under increased loads and handle intermittent failures more effectively. Subscribers can wait to pick up messages until off-peak hours, or messages can be routed or processed according to a specific schedule.
- __Improved testability__. Channels can be monitored and messages can be inspected or logged as part of an overall integration test strategy.
- __Separation of concerns__. Each application can focus on its core capabilities, while the messaging infrastructure handles everything required to reliably route messages to multiple consumers.
- __Integration__. It enables simpler integration between systems using different platforms, programming languages, or communication protocols.

Disadvantages
- __Inflexibility of data sent by publisher__. It introduces high semantic coupling in the messages passed by the publishers to the subscribers. Once the structure of the data is established, it becomes difficult to change.
Mitigation: Use versioned messaging format or versioned endpoints.
- __Instability of Delivery__. It is difficult to gauge the health of subscribers. The publisher does not know of the status of the subscribers. Messages may be lost. Mitigation: Introduce acknowledge of received messages.
- __Bottlenecks__. As a pub/sub system scales, the message broker often becomes a bottleneck for message flow.
Mitigation: consider the capabilities when chossing an existing messaging product according to the application’s requirements.

### Pattern Considerations
## Demo details
