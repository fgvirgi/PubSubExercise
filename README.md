# PubSubExercise
Small solution that demonstrates the concepts of the Publish/Subscribe pattern. 
[1. Requirements](#requirements)
[2. Demo details](#demo-details)
[3. Publish/Subscribe Pattern](#publishsubscribe-pattern)

## Requirements
Produce a small solution that demonstrates the concepts of the Publish/Subscribe pattern performing the followings:
- Take an input of data
- Transform that data in some way
- Transport that data to a set of subscribers
- Have the subscribers display the transformed data

## Demo details
For demonstration purpose consider an *University Course Registration System* used by:
- teaches to publish courses related to their specified domains
- students to subscribe to courses from the teachers they are interested in

The components are
- TeacherPublisherAPI: API used to publish courses
- TeacherPublishingDemo: console application used to exemplify multiple publisher teachers
- StudentSubscriber: worker service used to simulate the subscription and consuming process for multiple students.

The components are using RabbitMQ for underlying messaging transport.
__Note__: The implementation only focuses on exchanging messages. It can be further improved to consider security, message order, etc. [see points from the Consideration slide](#pattern-considerations). The design is flexible having the message client implemented through an abstraction interface ensuring easy exchange of a later client implementation with a different technology.


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
Consider the following points when deciding how to implement this pattern:
-- __Existing technologies.-- It is strongly recommended to use available messaging products and services that support a publish-subscribe model, rather than building your own. E.g. RabbitMQ, Azure Service Bus, Google Cloud Pub/Sub, Redis, Apache Kafka.
-- __Subscription handling.__ The messaging infrastructure must provide mechanisms that consumers can use to subscribe to or unsubscribe from available channels.
-- __Security. Connecting to any message channel must be restricted by security policy to prevent eavesdropping by unauthorized users or applications.
-- __Subsets of messages.__ Subscribers are usually only interested in subset of the messages distributed by a publisher. Consider usage of topics and content filtering (via attributes/routing keys)
-- __Wildcard subscribers.__ Consider allowing subscribers to subscribe to multiple topics via wildcards.
-- __Message ordering.__ The order in which consumer instances receive messages isn't guaranteed and doesn't necessarily reflect the order in which the messages were published. Some solutions may require that messages are processed in a specific order. 
-- __Poison messages.__ A malformed message can cause a service instance to fail. The system should prevent such messages being returned to the queue. Instead, capture and store the details of these messages elsewhere so that they can be analyzed if necessary.
-- __Repeated messages.__ The same message might be sent more than once. For example, the sender might fail after posting a message. Then a new instance of the sender might start up and repeat the message. You may need to implement duplicate message detection and removal.
-- __Message expiration.__ A message might have a limited lifetime. If it isn't processed within this period, it might no longer be relevant and should be discarded. A sender can specify an expiration time as part of the data in the message. A receiver can examine this information before deciding whether to perform the business logic associated with the message.
-- __Message scheduling.__ A message might be temporarily embargoed and should not be processed until a specific date and time. The message should not be available to a receiver until this time.

